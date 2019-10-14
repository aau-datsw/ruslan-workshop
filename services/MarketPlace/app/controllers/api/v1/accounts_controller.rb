class Api::V1::AccountsController < ApplicationController
  def index
    render json: current_account.as_json
                                .merge(stonk_count: current_account.stonk_count)
                                .merge(stonk_value: current_account.stonk_value)
                                .merge(total_value: current_account.total_value)
                                .symbolize_keys
                                .except(:id, :created_at, :updated_at, :api_key)
  end
end
