import os

def load_group_names(from_file):
    with open('group_names.txt' if from_file == '' else from_file) as fp: 
        return [name.strip() for name in fp.readlines()]

def select_group_name(group_names):
    print('Please enter the number of your group: ')
    for i, group in enumerate(group_names): 
        print(f'    {i+1}. {group}')

    choice = int(input())
    return group_names[choice-1]


def usr_bool(str): 
    return 'y' in str.lower()


def fill_template(template, args): 
  for key, value in args.items(): 
    template = template.replace(f'{{{key}}}', value)
  return template


def render_template(template_path=None, args=None): 
    with open(template_path) as t: 
        return fill_template(t.read(), args)
    

if __name__ == "__main__": 
    # Determine the group name
    group_names = load_group_names(input('  Please enter the file from which to load group names (default "group_names.txt"): ')) 
    selected_group = select_group_name(group_names)
    
    should_continue = usr_bool(input('The following operations will kill all running Docker containers. Continue? [Y/n]'))
    if not should_continue: 
        quit()

    # Kill all Docker containers, prepare for running their database on host
    os.system('sudo docker-compose down')
    database_name = f'{selected_group.lower()}-db'
    template_path = 'scaffolding/t-docker-compose-db.yml'
    with open('docker-compose-db.yml', 'w+') as fp: 
        fp.write(render_template(template_path=template_path, args={
            'database_name' : database_name
        }))

    # Run the database on host 5432 and attach this process to it 
    os.system('sudo docker-compose -f docker-compose-db.yml up -d')
    os.system(f'sudo dotnet ef migrations add NewMigration --project ./services/{selected_group}Service/{selected_group}API')
    os.system(f'sudo dotnet ef database update --project ./services/{selected_group}Service/{selected_group}API')
    os.system(f'sudo rm -rf services/{selected_group}Service/{selected_group}API/Migrations')

    
