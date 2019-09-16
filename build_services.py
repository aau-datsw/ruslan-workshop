import sys, os

# Provided a list of service names, this script builds the following: 
# 1. A .NET Core Web API for each project 
# 2. A PostgreSQL database for each project 
# 3. A docker-compose.yml file composing all projects
# 4. A docker-compose-local.yml file composing all projects for local development

def read_project_names(): 
  return [project_name for project_name in sys.argv[1:]]


# For every key X with a value Y in args, this function
# replaces all substrings in the template of the form '{X}' with 'Y'. 
def fill_template(template, args): 
  for key, value in args.items(): 
    template = template.replace(f'{{{key}}}', value)
  return template


# Function for building the seed.sql file for every 
# API that needs one.
def build_db_seed_v2(args={}, base_path=None): 
  if not os.path.exists(f'{base_path}/dbscripts'):
    os.mkdir(f'{base_path}/dbscripts')

  with open('templates/seed.sql.template') as t:
    content = fill_template(t.read(), args)
    with open(f'{base_path}/dbscripts/seed.sql', 'w+') as fp: 
      fp.write(content)


# Builds a new .NET Core 2.2 Web API project, creates dummy Models and a 
# database context and modifies Startup.cs so it is automatically connected 
# to its assigned PostgreSQL Docker container. 
def build_api_v2(args={}, base_path=None):
  project_path = f"./services/{args['project_name']}Service/{args['project_name']}API"
  models_path = f'{project_path}/Models'
  controllers_path = f'{project_path}/Controllers'
  
  # Create the project and add PostgreSQL as a dependency
  os.system(f"dotnet new webapi -o {project_path} --force --no-restore")
  os.system(f"dotnet add {project_path} package Npgsql.EntityFrameworkCore.PostgreSQL --no-restore")

  # Create the model(s) and database context
  if not os.path.exists(models_path): 
    os.mkdir(models_path)

  with open(f'{models_path}/Person.cs', 'w+') as fp: 
    with open(f'templates/Person.cs.template') as t: 
      fp.write(fill_template(t.read(), {
        'project_name' : args['project_name']
      }))

  with open(f'{models_path}/{project_name}APIContext.cs', 'w+') as fp: 
    with open(f'templates/DbContext.cs.template') as t: 
      fp.write(fill_template(t.read(), {
        'project_name' : args['project_name']
      }))

  # Correct the Startup.cs file  
  with open(f'{project_path}/Startup.cs', 'w+') as fp: 
    with open(f'templates/Startup.cs.template') as t: 
      fp.write(fill_template(t.read(), {
        'project_name' : args['project_name']
      }))

  # Delete the existing controller, create a new one 
  os.system(f'sudo rm {controllers_path}/ValuesController.cs')
  with open(f'{controllers_path}/{project_name}Controller.cs', 'w+') as fp: 
    with open(f'templates/Controller.cs.template') as t: 
      fp.write(fill_template(t.read(), {
        'project_name' : args['project_name']
      }))
  


# Creates the Dockerfile for a .NET Core 2.2 Web API. 
def build_dockerfile_v2(args={}, base_path=None, local=False): 
  with open('templates/Dockerfile.local.template' if local else 'templates/Dockerfile.template') as t: 
    with open(f'{base_path}/Dockerfile.local' if local else f'{base_path}/Dockerfile', 'w+') as fp: 
      fp.write(fill_template(t.read(), args))


# Builds a docker-compose.yml file (or a docker-compose-local.yml file if local=True) 
# that composes services for all provided names. For every name, a .NET Core 2.2 Web API 
# and an associated PostgreSQL database is created, each of which is run inside its own 
# Docker container. Each database uses its own volume. Configures an Nginx reverse proxy 
# that should redirect to all underlying services. 
def build_docker_compose(service_args=[], local=False, apis=[], volumes=[]): 
  service_template = None 
  services = ''
  depends_on_list = ''
  volume_list = ''

  with open('templates/docker-compose-service-local.template' if local else 'templates/docker-compose-service.template') as t: 
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

def build_restart(project_names=None, local=False): 
  if local:
    build_commands = []
    with open('templates/restart-local-build-api.template') as t: 
      for project_name in project_names: 
        build_commands.append(fill_template(t.read(), {
          'project_name' : project_name
        }))

    with open('templates/restart-local.sh.template') as t: 
      with open('restart-local.sh', 'w+') as fp: 
        fp.write(fill_template(t.read(), {
          'build_commands' : '\n'.join(build_commands)
        }))

  else:
    with open('templates/restart.sh.template') as t: 
      with open('restart.sh', 'w+') as fp: 
        fp.write(t.read())


# Main entrypoint
if __name__ == '__main__': 
  apis = []
  databases = []
  volumes = []
  service_paths = []

  # Should we rebuild the API projects or just create the docker-compose.yml file?
  user_choice = int(input(''' 
      Please select one of the following options: 
        [1] Create .NET APIs and Docker Compose files
        [2] Only create .NET APIs
        [3] Only create Docker Compose files
  '''))

  if user_choice not in [1, 2, 3]: 
    print('Choice must be a number. Aborting...')
    quit()

  # For all project names passed in the command line arguments, create an API project
  # (if so provided) and add various names to their corresponding lists so we can
  # build the docker-compose.yml file later.
  for project_name in read_project_names():
    service_path  = f'./services/{project_name}Service'
    database_name = f'{project_name.lower()}-db'
    api_name      = f'{project_name.lower()}-api'
    volume        = f'{database_name}-volume'

    apis.append(api_name)
    databases.append(database_name)
    volumes.append(volume)
    service_paths.append(service_path)

    if user_choice in [1, 2]:
      build_api_v2(args={
        'project_name' : project_name
      }, base_path='.')

      build_db_seed_v2(args={
        'database_name' : database_name
      }, base_path=service_path)

      build_dockerfile_v2(args={
        'api_name' : f'{project_name}API'
      }, base_path=service_path, local=False)

      build_dockerfile_v2(args={
        'api_name' : f'{project_name}API'
      }, base_path=service_path, local=True)

      # Restore with sudo 
      os.system(f'sudo dotnet restore {service_path}/{project_name}API/{project_name}API.csproj')


  if user_choice in [1, 3]: 
    # Create a docker-compose.yml file and write it to disk.
    build_docker_compose(service_args=[
      {
        'api_name' : api_name,
        'database_name' : database_name, 
        'volume' : volume,
        'service_path' : service_path
      }
    
      for api_name, database_name, volume, service_path in zip(apis, databases, volumes, service_paths)
    ], apis=apis, volumes=volumes)


    # Create a local one, too. 
    build_docker_compose(service_args=[
      {
        'api_name' : api_name,
        'database_name' : database_name, 
        'volume' : volume,
        'service_path' : service_path
      }
    
      for api_name, database_name, volume, service_path in zip(apis, databases, volumes, service_paths)
    ], apis=apis, volumes=volumes, local=True)

  build_restart(project_names=read_project_names(), local=True)
  build_restart()

  print(f'----------------------------------------------------------------')
  print(f'Summary:')
  if user_choice in [1, 2]: 
    print(f'    - Built .NET Core 2.2 Web APIs in the following directories:')
    for i, project_name in enumerate(read_project_names()):
      print(f'        {i+1}. ./services/{project_name}Service/{project_name}API')
  if user_choice in [1, 3]: 
    print(f'    - Created file:   ./docker-compose.yml')
    print(f'    - Created file:   ./docker-compose-local.yml')

  
