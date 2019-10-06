class Account < ApplicationRecord
  has_many :stonks
  has_many :transactions

  def balance(to=Time.current)
    self[:balance] + self.transactions.where(created_at: (100.years.ago..to)).sum("stonk_price * stonk_count * -1")
  end
end
