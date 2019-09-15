class ApiRequest
  BASE_URL = ENV.fetch('HOST', 'ruslan.local')

  class << self
    def get(url, params:, cookie:)
      {}
    end
  end
end
