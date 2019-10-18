class ApplicationController < ActionController::API
  before_action :require_authentication

  private

  def current_account
    @current_account ||= Account.find_by(api_key: request.headers['X-Token'])
  end

  def require_authentication
    return render(json: { error: { status: 401, code: 'unauthorized' } }, status: 401) if request.headers['X-Token'].blank? || current_account.blank?
  end
end
