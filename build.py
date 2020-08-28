import json, os, sys

'''
A script for scaffolding the RUSLAN Workshop server provided a list of names 
and a series of arguments determing which files to build. 
'''


# For every key X with a value Y in args, this function
# replaces all substrings in the template of the form '{X}' with 'Y'. 
def fill_template(template, args): 
  for key, value in args.items(): 
    template = template.replace(f'{{{{{key}}}}}', value)
  return template


def render_template(template_path=None, args=None): 
    with open(template_path) as t: 
        return fill_template(t.read(), args)


def usr_bool(user_answer): 
    return 'y' in user_answer.lower()


def load_group_keys(from_file=''): 
    with open('group_keys.json' if from_file == '' else from_file) as fp: 
        return json.load(fp)
    

def make_service_dir(name): 
    if os.path.exists(f'services/{name}Service'): 
        return 
    os.mkdir(f'services/{name}Service') 


def make_project_dir(name): 
    if os.path.exists(f'services/{name}Service/{name}'): 
        return 
    os.mkdir(f'services/{name}Service/{name}')


def make_dockerfile(name): 
    dockerfile = render_template('templates/Dockerfile.template', args={'name' : name})
    with open(f'services/{name}Service/Dockerfile', 'w') as fp: 
        fp.write(dockerfile)


def get_docker_compose_service(name): 
    return render_template('templates/docker-compose-service.template', args={'name' : name, 'name_lower' : name.lower()})
    

def make_docker_compose(names): 
    services = [get_docker_compose_service(name) for name in names]
    services = '\n\n\n'.join(services)
    docker_compose = render_template('templates/docker-compose.template', args={'services' : services})
    with open('docker-compose.yml', 'w') as fp: 
        fp.write(docker_compose)


def make_project(name, key): 
    project_path = f'services/{name}Service/{name}'
    program = render_template('templates/Program.cs.template', args={'name' : name})
    stonks = render_template('templates/Stonks.cs.template', args={'name' : name, 'key' : key})
    csproj = render_template('templates/Group.csproj.template', args={'name' : name})
    with open(f'{project_path}/Program.cs', 'w') as fp: 
        fp.write(program)
    with open(f'{project_path}/Stonks.cs', 'w') as fp: 
        fp.write(stonks)
    with open(f'{project_path}/{name}.csproj', 'w') as fp: 
        fp.write(csproj)
        


if __name__ == "__main__": 
    if len(sys.argv) >= 2 and sys.argv[1] == "-f":
        group_keys = load_group_keys('')
        should_build_projects = True
        should_restore_projects = True
        should_make_docker_compose = True
    else:
        group_keys = load_group_keys(input('Enter the group name-key file path (default is "group_keys.json"): '))
        should_build_projects = usr_bool(input('(Re)build .NET projects? This will overwrite existing projects. [Y/n]'))
        should_restore_projects = usr_bool(input('Restore .NET projects? [Y/n]'))
        should_make_docker_compose = usr_bool(input('(Re)build docker-compose.yml? This will overwrite the existing file. [Y/n]'))

    if should_build_projects:
        for name, key in group_keys.items(): 
            make_service_dir(name)
            make_dockerfile(name)
            make_project_dir(name)
            make_project(name, key)
            print(f'Built project for {name}...') 

    if should_restore_projects: 
        for name in group_keys: 
            os.system(f'dotnet restore services/{name}Service/{name}/{name}.csproj')

    if should_make_docker_compose: 
        make_docker_compose(group_keys)
        print('Built docker-compose.yml...')

    print('Done!')
