# This file is auto-generated from the current state of the database. Instead
# of editing this file, please use the migrations feature of Active Record to
# incrementally modify your database, and then regenerate this schema definition.
#
# This file is the source Rails uses to define your schema when running `rails
# db:schema:load`. When creating a new database, `rails db:schema:load` tends to
# be faster and is potentially less error prone than running all of your
# migrations from scratch. Old migrations may fail to apply correctly if those
# migrations use external dependencies or application code.
#
# It's strongly recommended that you check this file into your version control system.

ActiveRecord::Schema.define(version: 2019_09_15_112506) do

  create_table "accounts", force: :cascade do |t|
    t.string "api_key"
    t.string "name"
    t.integer "balance"
    t.datetime "created_at", precision: 6, null: false
    t.datetime "updated_at", precision: 6, null: false
    t.index ["api_key"], name: "index_accounts_on_api_key"
  end

  create_table "stonk_histories", force: :cascade do |t|
    t.integer "stonk_id", null: false
    t.integer "price"
    t.datetime "recorded"
    t.datetime "created_at", precision: 6, null: false
    t.datetime "updated_at", precision: 6, null: false
    t.index ["stonk_id"], name: "index_stonk_histories_on_stonk_id"
  end

  create_table "stonks", force: :cascade do |t|
    t.string "name"
    t.integer "price"
    t.datetime "created_at", precision: 6, null: false
    t.datetime "updated_at", precision: 6, null: false
  end

  create_table "transactions", force: :cascade do |t|
    t.integer "stonk_price"
    t.integer "stonk_count"
    t.datetime "recorded"
    t.integer "account_id", null: false
    t.integer "stonk_id", null: false
    t.datetime "created_at", precision: 6, null: false
    t.datetime "updated_at", precision: 6, null: false
    t.index ["account_id"], name: "index_transactions_on_account_id"
    t.index ["stonk_id"], name: "index_transactions_on_stonk_id"
  end

  add_foreign_key "stonk_histories", "stonks"
  add_foreign_key "transactions", "accounts"
  add_foreign_key "transactions", "stonks"
end
