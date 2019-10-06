import json

if __name__ == "__main__": 
    with open("market.json") as fp: 
        js = json.load(fp)
        min_price = min([o["y"] for o in js["data"]])

        with open("market_norm.json", "w+") as new_fp:
            json.dump({
                "name" : "Ligma Inc.", 
                "data" : [
                    {
                        "x" : i,
                        "y" : o["y"] + abs(min_price) + 1
                    }

                    for i, o in enumerate(js["data"])
                ]
            }, new_fp, indent=True)
