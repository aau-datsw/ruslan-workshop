class Api::V1::MarketsController < ApplicationController
  skip_before_action :require_authentication

  def index
    render json: StonkHistory.where(recorded: range_for_history).order(:recorded).select(:recorded, :price)
  end

  private

  def range_for_history
    (DateTime.parse(params[:from])..DateTime.parse(params[:to]))
  end
end
