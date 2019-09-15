import json
from random import random

CONFIG = {
    'volatile' : {
        'price_seed' : 2000,
        'change_rate' : 1000
    },
    'normal' : {
        'price_seed' : 10000,
        'change_rate' : 2000,
    },
    'slow' : {
        'price_seed' : 50000,
        'change_rate' : 5000
    }
}


if __name__ == "__main__": 
    companies = []
    company_names = None 
    with open('company_names.json') as fp: 
        company_names = [o['name'] for o in json.load(fp)]
    
    print(f'Loaded {len(company_names)} company names.')
    group_size = int(len(company_names) * 1/3)

    volatile_companies = company_names[:group_size]
    slow_companies = company_names[group_size:group_size * 2]
    normal_companies = company_names[group_size * 2:]

    for volatility, company_group in {
        'volatile' : volatile_companies,
        'normal' : normal_companies,
        'slow' : slow_companies
    }.items(): 
        for name in company_group: 
            price_seed = CONFIG[volatility]['price_seed']
            variance = random() * CONFIG[volatility]['change_rate']
            price = price_seed + variance if random() > 0.5 else price_seed - variance
            companies.append({
                'name' : name,
                'price' : price,
                'volatility' : ['volatile', 'normal', 'slow'].index(volatility)
            })

    with open('companies.json', 'w+') as fp: 
        json.dump(companies, fp, indent=4, sort_keys=True)
