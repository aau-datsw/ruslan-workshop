class CreateStonkHistories < ActiveRecord::Migration[6.0]
  def change
    create_table :stonk_histories do |t|
      t.references :stonk, null: false, foreign_key: true
      t.integer :price
      t.datetime :recorded

      t.timestamps
    end
  end
end
