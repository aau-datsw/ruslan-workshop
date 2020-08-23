class StonkHistory < ApplicationRecord
  belongs_to :stonk

  def self.current
    self.where("recorded < ?", Time.zone.now).order(:recorded).last
  end
end
