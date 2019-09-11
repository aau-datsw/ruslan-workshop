import sys, os

# Provided a list of service names, this script builds the following: 
# 1. A .NET Core Web API for each project 
# 2. A PostgreSQL database for each project 
# 3. A docker-compose.yml file composing all projects
# 4. A docker-compose-local.yml file composing all projects for local development

def read_project_names(): 
  return [project_name for project_name in sys.argv[1:]]


def fill_template(template, args): 
  for key, value in args.items(): 
    template = template.replace(f'{{{key}}}', value)
  return template


def build_db_seed_v2(args={}, base_path=None): 
  if not os.path.exists(f'{base_path}/dbscripts'):
    os.mkdir(f'{base_path}/dbscripts')

  with open('templates/seed.sql.template') as t:
    content = fill_template(t.read(), args)
    with open(f'{base_path}/dbscripts/seed.sql', 'w+') as fp: 
      fp.write(content)


def build_api_v2(args={}, base_path=None):
  os.system(f"dotnet new webapi -o Services/{args['project_name']}Service/{args['project_name']}API --force")


def build_dockerfile_v2(args={}, base_path=None): 
  with open('templates/Dockerfile.template') as t: 
    with open(f'{base_path}/Dockerfile', 'w+') as fp: 
      fp.write(fill_template(t.read(), args))


def build_docker_compose(service_args=[], local=False, apis=[], volumes=[]): 
  service_template = None 
  services = ''
  depends_on_list = ''
  volume_list = ''

  with open('templates/docker-compose-service.template') as t: 
    service_template = t.read()

  for args in service_args: 
    services += f'{fill_template(service_template, args)}\n'
  
  for api in apis: 
    depends_on_list += f'      - {api}\n'

  for volume in volumes: 
    volume_list += f'  {volume}:\n'

  with open('templates/docker-compose-local.yml.template' if local else 'templates/docker-compose.yml.template') as t: 
    with open('docker-compose-local.yml' if local else 'docker-compose.yml', 'w+') as fp: 
      fp.write(fill_template(t.read(), {
          'depends_on_list' : depends_on_list,
          'services' : services,
          'volume_list' : volume_list
        })
      )
            
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

    apis.append(api_name)
    databases.append(database_name)
    volumes.append(volume)
    service_paths.append(service_path)
  
    build_api_v2(args={
      'project_name' : project_name
    }, base_path='.')

    build_db_seed_v2(args={
      'database_name' : database_name
    }, base_path=service_path)

    build_dockerfile_v2(args={
      'api_name' : f'{project_name}API'
    }, base_path=service_path)


  build_docker_compose(service_args=[
    {
      'api_name' : api_name,
      'database_name' : database_name, 
      'volume' : volume,
      'service_path' : service_path
    }
  
    for api_name, database_name, volume, service_path in zip(apis, databases, volumes, service_paths)
  ], apis=apis, volumes=volumes)


  build_docker_compose(service_args=[
    {
      'api_name' : api_name,
      'database_name' : database_name, 
      'volume' : volume,
      'service_path' : service_path
    }
  
    for api_name, database_name, volume, service_path in zip(apis, databases, volumes, service_paths)
  ], apis=apis, volumes=volumes, local=True)
