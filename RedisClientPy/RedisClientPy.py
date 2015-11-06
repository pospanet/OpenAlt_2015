import redis
import json

class Person:
    def __init__(self, firstName = '', familyName = ''):
        self.FirstName = firstName
        self.FamilyName = familyName    
    def fromBinaryJson(self, jsonData):
        value = str(jsonData)
        value = value[2:len(value)-1]
        data = json.loads(value)
        self.FirstName = data['FirstName']
        self.FamilyName = data['FamilyName']
    def fromJson(self, jsonData):
        data = json.loads(jsonData)
        self.FirstName = data.FirstName
        self.FamilyName = data.FamilyName
    def toString(self):
        return self.FirstName + ' ' + self.FamilyName
    def toJson(self):
        return json.dumps(self, default=lambda o: o.__dict__, 
            sort_keys=True)

me = Person(firstName = 'Jan', familyName = 'Pospisil')

print(me.toString())

print(me.toJson())

cache = redis.StrictRedis(host='<host_name>.redis.cache.windows.net',
      port=6380, db=0, password='<access_key>', ssl=True)

cache.set('foo', 'bar')

value = cache.get('foo')

#ex seconds
#px miliseconds
#nx only if not exist
#xx only if exist

cache.set('p007',me.toJson(),ex=3600)

value = cache.get('p007')

meFromCache = Person()
meFromCache.fromBinaryJson(value)

print(meFromCache.toString())

print(meFromCache.toJson())
