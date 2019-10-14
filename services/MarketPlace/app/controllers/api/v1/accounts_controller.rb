class Api::V1::AccountsController < ApplicationController
  def index
    render json: current_account
  end
end
