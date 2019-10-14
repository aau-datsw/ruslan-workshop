class Transaction < ApplicationRecord
  belongs_to :account
  belongs_to :stonk

  before_validation :set_default_count

  validates_presence_of :stonk_count
  validates_presence_of :stonk_price

  validates :stonk_price, numericality: { greater_than: 0 }

  private

  def set_default_count
    self.stonk_count ||= 1
  end
end
