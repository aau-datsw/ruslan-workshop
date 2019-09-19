import json, os

'''
A script for scaffolding the RUSLAN Workshop server provided a list of names 
and a series of arguments determing which files to build. 
'''


# For every key X with a value Y in args, this function
# replaces all substrings in the template of the form '{X}' with 'Y'. 
def fill_template(template, args): 
  for key, value in args.items(): 
    template = template.replace(f'{{{key}}}', value)
  return template


def render_template(template_path=None, args=None): 
    with open(template_path) as t: 
        return fill_template(t.read(), args)


def load_group_names(file_path=''): 
    with open('group_names.txt' if file_path == '' else file_path) as fp: 
        return fp.readlines()


def usr_bool(user_answer): 
    return 'y' in user_answer.lower()


def build_docker_compose_services(api_names, database_names, volume_names, service_names, local=False, db_config=False): 
    services = []
    for api, db, volume, service in zip(api_names, database_names, volume_names, service_names): 
        service = f'''
        {render_template(
            template_path=
                'scaffold/misc/t-docker-compose-service-db' if db_config 
                else 'scaffold/misc/t-docker-compose-service-local' if local 
                else 'scaffold/misc/t-docker-compose-service',
            args={
                'database_name' : db, 
                'api_name' : api, 
                'volume' : volume,
                'service_path' : f'services/{service}'
            })}
        '''
        services.append(service)

    return '\n \n'.join(services)


def build_docker_compose_depends_on_list(api_names): 
    return '\n'.join([f'      - {api}' for api in api_names])


def build_docker_compose_volume_list(volumes): 
    return '\n'.join([f'  - {volume}:' for volume in volumes])


def build_docker_compose(api_names, database_names, volume_names, service_names, local=False, db_config=False): 
    services = build_docker_compose_services(api_names, database_names, volume_names, service_names, local=local, db_config=db_config)
    depends_on_list = build_docker_compose_depends_on_list(database_names if db_config else api_names)
    volume_list = build_docker_compose_volume_list(volume_names)

    build_path = 'docker-compose-db.yml' if db_config else 'docker-compose-db.yml' if local else 'docker-compose-db.yml'
    template_path = f'scaffolding/t-{build_path}'

    with open(build_path, 'w+') as fp: 
        fp.write(render_template(template_path=template_path, args={
            'services' : services,
            'depends_on_list' : depends_on_list, 
            'volume_list' : volume_list
        }))        

    print(f'    - (Docker Compose) Built {build_path} composing {len(api_names)} services...')

    
def build_dockerfiles(api_names, service_names, local=False): 
    for service_name, api_name in zip(service_names, api_names): 
        build_path = f'{service_name}/Dockerfile.local' if local else f'{service_name}/Dockerfile'
        template_path = 'scaffolding/services/Dockerfile.local' if local else 'scaffolding/services/Dockerfile'

        with open(build_path, 'w+') as fp: 
            fp.write(render_template(template_path=template_path, args={
                'api_name' : api_name
            }))

    print(f'    - (Docker) Built {"Dockerfile.local" if local else "Dockerfile"} for {len(api_names)} projects...')


def build_web_api(service_name, api_name, database_name): 
    project_path = f'services/{service_name}/{api_name}'
    models_path = f'{project_path}/Models'
    controllers_path = f'{project_path}/Controllers'

    startup = {
        'build' : f'{project_path}/Startup.cs',
        'template' : f'scaffolding/services/api/t-Startup.cs'
    }

    extensions = {
        'build' : f'{project_path}/{api_name}Extensions.cs',
        'template' : 'scaffolding/services/api/t-APIExtensions.cs'
    }

    appsettings = {
        'build' : f'{project_path}/appsettings.json', 
        'template' : 'scaffolding/services/api/t-appsettings.json'
    }

    model = {
        'build' : f'{models_path}/Company.cs', 
        'template' : 'scaffolding/services/api/Models/t-Company.cs'
    }

    context = {
        'build' : f'{models_path}/{api_name}Context.cs', 
        'template' : 'scaffolding/services/api/Models/t-APIContext.cs'
    }

    context_factory = {
        'build' : f'{models_path}/{api_name}ContextFactory.cs', 
        'template' : 'scaffolding/services/api/Models/t-APIContextFactory.cs'
    }

    controller = {
        'build' : f'{controllers_path}/{api_name}Controller.cs',
        'template' : 'scaffolding/services/api/Controllers/t-Controller.cs'
    }

    os.system(f"dotnet new webapi -o {project_path} --force --no-restore")
    os.system(f"dotnet add {project_path} package Npgsql.EntityFrameworkCore.PostgreSQL --no-restore")
    os.system(f"dotnet add {project_path} package Microsoft.EntityFrameworkCore.Tools.DotNet -v 2.2.6 --no-restore")
    os.system(f"sudo rm {controllers_path}/ValuesController.cs")
    

    with open(startup['build'], 'w+') as fp: 
        fp.write(render_template(template_path=startup['template'], args={
            'api_name' : api_name
        }))

    with open(extensions['build'], 'w+') as fp: 
        fp.write(render_template(template_path=extensions['template'], args={
            'api_name' : api_name
        }))

    with open(appsettings['build'], 'w+') as fp: 
        fp.write(render_template(template_path=appsettings['template'], args={
            'database_name' : database_name
        }))

    with open(model['build'], 'w+') as fp: 
        fp.write(render_template(template_path=model['template'], args={
            'api_name' : api_name
        }))

    with open(context['build'], 'w+') as fp: 
        fp.write(render_template(template_path=context['template'], args={
            'api_name' : api_name
        }))

    with open(context_factory['build'], 'w+') as fp: 
        fp.write(render_template(template_path=context_factory['template'], args={
            'api_name' : api_name
        }))

    with open(controller['build'], 'w+') as fp: 
        fp.write(render_template(template_path=controller['template'], args={
            'api_name' : api_name
        }))


def build_web_apis(services_names, api_names, database_names): 
    for service, api, db in zip(services_names, api_names, database_names): 
        build_web_api(service, api, db)

    print(f'    - (ASP.NET Core) Built {len(service_names)} .NET Core APIs...')


def build_nginx_locations(api_names, api_urls): 
    locations = []
    template_path = 'scaffolding/misc/t-nginx-location'
    for api, url in zip(api_names, api_urls): 
        locations.append(render_template(template_path=template_path), args={
            'api_name' : api, 
            'api_url' : url
        })

    return '\n \n'.join(locations)


def build_nginx(api_names, api_urls, local=False): 
    locations = build_nginx_locations(api_names, api_urls)
    build_path = 'nginx.conf.local' if local else 'nginx.conf'
    template_path = 'scaffolding/t-nginx.conf.local' if local else 't-nginx.conf'

    with open(build_path, 'w+') as fp: 
        fp.write(render_template(template_path=template_path, args={
            'api_locations' : locations
        }))

    print(f'    - (Nginx) Built {build_path} proxying {len(api_names)} APIs...')
    




# Main entrypoint
if __name__ == "__main__": 
    group_names = load_group_names(input('Group name file path (default is "group_names.txt"): '))

    service_names =    [f'{group_name}Service' for group_name in group_names]
    api_names =        [f'{group_name}API' for group_name in group_names]
    docker_api_names = [f'{group_name.lower()}-api' for group_name in group_names]
    database_names =   [f'{group_name.lower()}-db' for group_name in group_names]
    volume_names =     [f'{group_name.lower()}-db-volume' for group_name in group_names]
    api_urls =         [f'{api.replace("-api", "")}' for api in docker_api_names]

    should_build_docker = usr_bool(input('''
        Should I build all Docker files for the projects (docker-compose.yml, docker-compose-local.yml, 
        docker-compose-db.yml, Dockerfile and Dockerfile.local for every project? [Y/n])'''))
    should_build_dotnet_projects = usr_bool(input(''' 
        Should I (re)build all .NET Web APIs, potentially overwriting existing projects? [Y/n]'''))
    should_build_nginx = usr_bool(input(''' 
        Should I build the nginx.conf and nginx.conf.local files redirecting to all projects? [Y/n]'''))

    if should_build_docker: 
        build_docker_compose(docker_api_names, database_names, volume_names, service_names)
        build_docker_compose(docker_api_names, database_names, volume_names, service_names, local=True)
        build_docker_compose(docker_api_names, database_names, volume_names, service_names, db_config=True)

        build_dockerfiles(api_names, service_names)
        build_dockerfiles(api_names, service_names, local=True)


    if should_build_nginx: 
        build_nginx(api_names, api_urls)
        build_nginx(api_names, api_urls, local=True)

    if should_build_dotnet_projects: 
        build_web_apis(service_names, api_names, database_names) 
        
    
    print('Completed scaffolding server for the following groups:')
    for i, group_name in enumerate(group_names): 
        print(f'    {i+1}. {group_name}')