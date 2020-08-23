require 'net/http'
# This file should contain all the record creation needed to seed the database with its default values.
# The data can then be loaded with the rails db:seed command (or created alongside the database with db:setup).
#
# Examples:
#
#   movies = Movie.create([{ name: 'Star Wars' }, { name: 'Lord of the Rings' }])
#   Character.create(name: 'Luke', movie: movies.first)

# Load stonks
Stonk.create(
  name: "TheDonald A/S",
  price: 0
)

resp = Net::HTTP.get_response(URI.parse("https://raw.githubusercontent.com/aau-datsw/ruslan-workshop/master/data/market_norm.json"))
data = resp.body
result = JSON.parse(data)
stonk = Stonk.default_stonk
arr = []

year = (ENV["YEAR"] || "2019").to_i
month = (ENV["MONTH"] || "10").to_i
day = (ENV["DAY"] || "18").to_i
hour = 20

result['data'].each do |data|
  arr << {
    price: data['y'],
    stonk_id: stonk.id,
    recorded: DateTime.new(year, month, day, hour).in_time_zone("Europe/Copenhagen") + data['x'].seconds
  }
end

StonkHistory.import(arr)

# Load accounts
resp = Net::HTTP.get_response(URI.parse("https://raw.githubusercontent.com/aau-datsw/ruslan-workshop/master/group_keys.json"))
data = resp.body
result = JSON.parse(data)
arr = []

result.each do |name, key|
  arr << Account.new(
    api_key: key,
    name: name,
    balance: 100000
  )
end

Account.import(arr)