class CreateStonks < ActiveRecord::Migration[6.0]
  def change
    create_table :stonks do |t|
      t.string :name
      t.integer :price

      t.timestamps
    end
  end
end
