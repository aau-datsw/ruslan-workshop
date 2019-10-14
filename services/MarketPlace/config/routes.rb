Rails.application.routes.draw do
  namespace :api do
    namespace :v1 do
      post 'sell', to: 'transactions#sell'
      post 'buy', to: 'transactions#buy'
      get 'account', to: 'accounts#index'
    end
  end

  mount Rswag::Ui::Engine => '/api-docs'
  mount Rswag::Api::Engine => '/docs'
  # For details on the DSL available within this file, see https://guides.rubyonrails.org/routing.html
end
