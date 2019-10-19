import matplotlib.pyplot as plt

#Få data til at virke
reader = open("STONKS.txt","r")
data_unsorted = reader.read()
reader.close()
data=[]
for i in data_unsorted:
    try:
        data.append(int(i))
    except ValueError:
        pass

print(data)

tid = []
for n in range(len(data)):
    tid.append(n*30)
    
plt.plot(tid,data)
plt.ylabel("Aktiepris")
plt.xlabel("Tid i sekunder")