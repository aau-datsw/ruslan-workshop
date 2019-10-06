require 'rails_helper'

RSpec.describe Transaction, type: :model do
  fixtures :all

  describe 'Transaction' do
    it 'updates account balance' do
      acc = Account.create(name: 'tester', balance: 5000)
      stonk = Stonk.default_stonk

      expect {
        acc.transactions.create(stonk_price: stonk.price, stonk: stonk, stonk_count: 1)
      }.to change { acc.reload.balance }.by(-stonk.price)

      expect {
        acc.transactions.create(stonk_price: stonk.price, stonk: stonk, stonk_count: 2)
      }.to change { acc.reload.balance }.by(stonk.price * -2)

      expect(acc.balance(1.minute.ago)).to equal(5000)
      expect(acc.balance).to equal(842)
    end
  end
end
