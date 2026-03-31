using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToDoApp.Services
{
    // --- API RESPONSE WRAPPERS & MODELS ---
    
    // Generic wrapper to handle the API's standard { status, message, data } format
    public class ApiResponse<T>
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public T Data { get; set; }
    }

    // Model for the SignIn response
    public class User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("fname")]
        public string FirstName { get; set; }

        [JsonPropertyName("lname")]
        public string LastName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }

    // --- THE MAIN API SERVICE ---

    public class TodoApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://todo-list.dcism.org";

        public TodoApiService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        // ==========================================
        // AUTHENTICATION
        // ==========================================

        public async Task<(bool IsSuccess, string Message)> SignUpAsync(string fname, string lname, string email, string password)
        {
            // Ensure the keys exactly match the API documentation
            var payload = new 
            { 
                first_name = fname, 
                last_name = lname, 
                email = email, 
                password = password, 
                confirm_password = password 
            };

            try 
            {
                var response = await _httpClient.PostAsJsonAsync("/signup_action.php", payload);
        
                // Read the raw text from the server, no matter what happens
                string rawContent = await response.Content.ReadAsStringAsync();
        
                var result = JsonSerializer.Deserialize<ApiResponse<object>>(rawContent);
        
                if (result != null && result.Status == 200)
                {
                    return (true, result.Message);
                }
        
                // If it fails, grab the EXACT message the API sent back
                return (false, result?.Message ?? rawContent); 
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<User> SignInAsync(string email, string password)
        {
            var response = await _httpClient.GetAsync($"/signin_action.php?email={email}&password={password}");
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<User>>();
            
            return result?.Status == 200 ? result.Data : null;
        }

        // ==========================================
        // TO-DO OPERATIONS
        // ==========================================

        public async Task<List<ToDoClass>> GetTasksAsync(int userId, string status = "active")
        {
            var response = await _httpClient.GetAsync($"/getItems_action.php?status={status}&user_id={userId}");
            
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                
                // The API returns a JSON object with keys "0", "1", etc., instead of a standard array.
                // We parse it into a Dictionary first, then grab the values.
                var result = JsonSerializer.Deserialize<ApiResponse<Dictionary<string, ToDoClass>>>(jsonString);
                
                if (result?.Status == 200 && result.Data != null)
                {
                    return result.Data.Values.ToList();
                }
            }
            return new List<ToDoClass>();
        }

        public async Task<ToDoClass> AddTaskAsync(string name, string description, int userId)
        {
            var payload = new { item_name = name, item_description = description, user_id = userId };
            var response = await _httpClient.PostAsJsonAsync("/addItem_action.php", payload);
            
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ToDoClass>>();
            return result?.Status == 200 ? result.Data : null;
        }

        public async Task<bool> UpdateTaskAsync(int itemId, string name, string description)
        {
            var payload = new { item_id = itemId, item_name = name, item_description = description };
            var response = await _httpClient.PutAsJsonAsync("/editItem_action.php", payload);
            
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
            return result?.Status == 200;
        }

        public async Task<bool> ChangeTaskStatusAsync(int itemId, string newStatus)
        {
            var payload = new { item_id = itemId, status = newStatus };
            var response = await _httpClient.PutAsJsonAsync("/statusItem_action.php", payload);
            
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
            return result?.Status == 200;
        }

        public async Task<bool> DeleteTaskAsync(int itemId)
        {
            // Method is DELETE, but data is passed via URL query parameters
            var response = await _httpClient.DeleteAsync($"/deleteItem_action.php?item_id={itemId}");
            
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
            return result?.Status == 200;
        }
    }
}