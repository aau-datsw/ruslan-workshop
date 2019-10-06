class CreateAccounts < ActiveRecord::Migration[6.0]
  def change
    create_table :accounts do |t|
      t.string :api_key
      t.string :name
      t.integer :balance

      t.timestamps
    end
    add_index :accounts, :api_key
  end
end
