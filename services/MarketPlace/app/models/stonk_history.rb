class StonkHistory < ApplicationRecord
  belongs_to :stonk

  def self.current
    self.where("recorded < NOW()").order(:recorded).last
  end
end
