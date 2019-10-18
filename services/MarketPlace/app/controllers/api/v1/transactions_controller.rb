class Api::V1::TransactionsController < ApplicationController
  before_action :set_stonk
  before_action :validate_params

  def buy
    transaction = nil
    ActiveRecord::Base.transaction do
      price = @stonk.price
      transaction = current_account.transactions.create(
        stonk: @stonk,
        stonk_price: price,
        stonk_count: (current_account.balance / price.to_f).floor
      )
    end

    render json: transaction, status: 201
  end

  def sell
    transaction = nil
    ActiveRecord::Base.transaction do
      transaction = current_account.transactions.create(stonk: @stonk, stonk_price: @stonk.price, stonk_count: -1 * current_account.stonk_count)
      raise "Not enough money" if current_account.balance < 0
      raise "Not enough stonk" if current_account.stonk_count < 0
    end

    render json: transaction, status: 201
  rescue RuntimeError => e
    render json: { error: e.message }
  end

  private

  def quantity
    return params[:quantity].to_i if params[:quantity].present?
    current_account.stonk_count
  end

  def validate_params
    if params.dig(:quantity).blank?
      params[:quantity] = 1
    end
    return head(422) if params.dig(:quantity) && params.dig(:quantity).to_i < 1
  end

  def set_stonk
    @stonk ||= Stonk.default_stonk
  end
end
