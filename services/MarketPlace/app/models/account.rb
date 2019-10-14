class Account < ApplicationRecord
  has_many :stonks
  has_many :transactions

  def balance(to=Time.current)
    self[:balance] + self.transactions.where(created_at: (100.years.ago..to)).sum("(stonk_price * -1) * stonk_count")
  end

  def stonk_value
    self.transactions.sum(:stonk_count) * StonkHistory.current.price
  end

  def total_value
    self.balance + self.stonk_value
  end

  def stonk_count
    self.transactions.sum(:stonk_count)
  end
end
