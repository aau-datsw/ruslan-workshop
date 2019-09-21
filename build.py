import os 

if __name__ == "__main__": 
    service_names = os.listdir("./services/")
    group_names =   [name.replace("Service", "") for name in service_names]
    api_names =     [f'{name}API' for name in group_names]

    for service_name, api_name in zip(service_names, api_names): 
        os.system(f'sudo dotnet publish services/{service_name}/{api_name}/{api_name}.csproj -c Debug')
    
