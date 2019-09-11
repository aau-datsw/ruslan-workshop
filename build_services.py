import sys, os

# Provided a list of service names, this script builds the following: 
# 1. A .NET Core Web API for each project 
# 2. A PostgreSQL database for each project 
# 3. A docker-compose.yml file composing all projects
# 4. A docker-compose-local.yml file composing all projects for local development

def read_project_names(): 
    return [project_name for project_name in sys.argv[1:]]

def build_db_seed(database_name, base_path=None): 
    if not os.path.exists(f'{base_path}/dbscripts'):
        os.mkdir(f'{base_path}/dbscripts')

    with open(f'{base_path}/dbscripts/seed.sql', 'w+') as fp: 
        fp.write(f''' 
\\connect {database_name}

CREATE TABLE people (
id          SERIAL       PRIMARY KEY,
name        TEXT,
other_name  TEXT
) WITH (OIDS=FALSE);

ALTER TABLE people OWNER TO ya;

INSERT INTO people(name, other_name) VALUES(
'Anders Brams',
'The literal god'
);
''')

    
def build_api(project_name, base_path=None): 
    os.system(f'dotnet new webapi -o {base_path}/{project_name}Service/{project_name}API --force')


def build_dockerfile(project_name, base_path=None): 
    api_name = f'{project_name}API'
    with open(f'{base_path}/Dockerfile', 'w+') as fp: 
        fp.write(f''' 
FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY ./{api_name}/*.csproj ./
RUN dotnet restore {api_name}.csproj
COPY ./{api_name}/. ./
RUN dotnet publish {api_name}.csproj -c Release -o pub

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=0 /app/pub .
ENTRYPOINT ["dotnet", "{api_name}.dll"]
''')



if __name__ == '__main__': 

    apis = []
    databases = []
    volumes = []
    service_paths = []

    for project_name in read_project_names():

        service_path  = f'./Services/{project_name}Service'
        database_name = f'{project_name.lower()}-db'
        api_name      = f'{project_name.lower()}-api'
        volume        = f'{database_name}-volume'

        build_api(project_name, base_path='./Services')
        build_db_seed(database_name, base_path=service_path)
        build_dockerfile(project_name, base_path=service_path)

        apis.append(api_name)
        databases.append(database_name)
        volumes.append(volume)
        service_paths.append(service_path)


    # Construct the preamble that creates the Nginx reverse proxy
    # and lists its dependencies
    preamble = f''' 
version: "3"

networks:
  ruslan-workshop-shared:

services: 
  reverse-proxy:
    image: nginx:latest
    volumes:
      - ./nginx.conf:/etc/nginx/nginx.conf
      - /etc/letsencrypt/:/etc/letsencrypt/
    ports:
      - "80:80"
      - "443:443"
    networks:
      - ruslan-workshop-shared
    depends_on:
'''

    # ... and the preamble for the local version
    preamble_local = f''' 
version: "3"

networks:
  ruslan-workshop-shared:

services: 
  reverse-proxy:
    image: nginx:latest
    volumes:
      - ./nginx.conf.local:/etc/nginx/nginx.conf
      - /etc/letsencrypt/:/etc/letsencrypt/
    ports:
      - "80:80"
      - "443:443"
    networks:
      - ruslan-workshop-shared
    depends_on:
'''

    depends_on_list = str('\n'.join([f'      - {api_name}' for api_name in apis]))


    # Then construct the services - a service is a database and 
    # and API that uses the database
    services = str('\n'.join([
        f''' 
  {database_name}: 
    image: postgres:latest
    expose: 
      - 5432
    restart: always
    volumes: 
      - {service_path}/dbscripts/seed.sql:/docker-entrypoint-initdb.d/seed.sql
      - {volume}:/var/lib/postgresql/data
    environment:
      POSTGRES_USER: "ya"
      POSTGRES_PASSWORD: "yeet"
      POSTGRES_DB: "{database_name}"
    networks:
      - ruslan-workshop-shared
    
  {api_name}:
    image: {api_name}:latest
    container_name: {api_name}
    build:
      context: {service_path}
      dockerfile: ./Dockerfile
    expose:
      - 80
    environment:
      DB_CONNECTION_STRING: "host={database_name};port=5432;database={database_name};username=ya;password=yeet"
    depends_on:
      - {database_name}
    networks:
      - ruslan-workshop-shared        
'''
    for api_name, database_name, volume, service_path in zip(apis, databases, volumes, service_paths)]))


    # Finally, provide the volumes that are going to be used by
    # the databases
    volume_list = str('\n'.join([
        f'''  {volume}: '''
    for volume in volumes]))

    db_volumes = f''' 
volumes:
{volume_list}
    '''


    with open('docker-compose.yml', 'w+') as fp: 
        fp.write(f''' 
{preamble}
{depends_on_list}
{services}
{db_volumes}
''')

    with open('docker-compose-local.yml', 'w+') as fp:
      fp.write(f''' 
{preamble_local}
{depends_on_list}
{services}
{db_volumes}
''')