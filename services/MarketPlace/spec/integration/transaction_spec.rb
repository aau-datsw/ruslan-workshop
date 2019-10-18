require 'swagger_helper'

describe 'Transaction API' do
  fixtures :stonks

  before do
    @account = Account.create(api_key: '12345', name: 'Test', balance: 0)
  end

  path '/api/v1/buy' do
    post 'Buy Stonks' do
      tags 'Transactions'
      security [ "api_header" => {} ]
      consumes 'application/json'
      produces 'application/json'

      parameter name: 'X-Token', in: :header, type: :string

      parameter name: :transaction, in: :body, schema: {
        type: :object,
        properties: {
          quantity: { type: :integer }
        },
        required: ['quantity']
      }

      response '201', 'created' do
        let(:transaction) { { quantity: 5 } }
        let(:'X-Token') { @account.api_key }

        run_test!
      end

      response '422', 'invalid request' do
        let(:transaction) { { quantity: -5 } }
        let(:'X-Token') { @account.api_key }

        run_test!
      end

      response '401', 'unauthorized' do
        let(:transaction) { {} }
        let(:'X-Token') { '1234' }

        run_test!
      end
    end
  end

  path '/api/v1/sell' do
    post 'Sell Stonks' do
      tags 'Transactions'
      security [ "api_header" => {} ]
      consumes 'application/json'
      produces 'application/json'

      parameter name: 'X-Token', in: :header, type: :string
      parameter name: :transaction, in: :body, schema: {
        type: :integer,
        properties: {
          quantity: { type: :integer }
        },
        required: ['quantity']
      }

      response '201', 'created' do
        let(:transaction) { { quantity: 5 } }
        let(:'X-Token') { @account.api_key }

        run_test!
      end

      response '422', 'invalid request' do
        let(:transaction) { { quantity: -5 } }
        let(:'X-Token') { @account.api_key }

        run_test!
      end
    end
  end

end
