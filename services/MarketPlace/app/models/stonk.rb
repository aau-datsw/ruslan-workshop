class Stonk < ApplicationRecord
  has_many :stonk_histories
  has_many :transactions

  def self.default_stonk
    Stonk.last
  end

  def price
    StonkHistory.current.price
  end
end
