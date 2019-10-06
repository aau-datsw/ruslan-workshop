class CreateTransactions < ActiveRecord::Migration[6.0]
  def change
    create_table :transactions do |t|
      t.integer :stonk_price
      t.integer :stonk_count

      t.datetime :recorded

      t.references :account, null: false, foreign_key: true
      t.references :stonk, null: false, foreign_key: true

      t.timestamps
    end
  end
end
