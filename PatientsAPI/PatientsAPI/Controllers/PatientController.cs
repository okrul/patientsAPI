using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace PatientsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly string _connectionString = "server=localhost;port=3306;database=api_test;uid=root;pwd=1qaz@WSX3edc;";

        [HttpGet("GetPatients")]
        public async Task<IEnumerable<PatientWithId>> GetPatients()
        {
            List<PatientWithId> patients = new List<PatientWithId>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sqlText = "SELECT * FROM Patient";
                using var command = new MySqlCommand(sqlText, connection);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    patients.Add(new PatientWithId
                    {
                        Name = new NameWithId
                        {
                            Id = reader.GetGuid("Id"),
                            Use = reader.GetString("UseName"),
                            Family = reader.GetString("FamilyName"),
                            Given = reader.GetString("GivenName").Split(' ')
                        },
                        Gender = reader.GetString("Gender"),
                        BirthDate = reader.GetDateTime("BirthDate"),
                        Active = reader.GetBoolean("Active")
                    });
                }
            }
            return patients;
        }

        [HttpGet("GetPatient{id}")]
        public async Task<ActionResult<PatientWithId?>> GetPatient(Guid id)
        {
            PatientWithId patient = null;
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = "SELECT * FROM Patient WHERE Id = @Id";
                using var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    patient = new PatientWithId
                    {
                        Name = new NameWithId
                        {
                            Id = reader.GetGuid("Id"),
                            Use = reader.GetString("UseName"),
                            Family = reader.GetString("FamilyName"),
                            Given = reader.GetString("GivenName").Split(' ')
                        },
                        Gender = reader.GetString("Gender"),
                        BirthDate = reader.GetDateTime("BirthDate"),
                        Active = reader.GetBoolean("Active")
                    };
                }
            }
            return patient;
        }

        [HttpPost("PostPatient")]
        public async Task<IActionResult> PostPatient(Patient patient)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sqlText = "INSERT INTO Patient (UseName, Gender, Active, FamilyName, GivenName, BirthDate) " +
                             "VALUES (@UseName, @Gender, @Active, @FamilyName, @GivenName, @BirthDate)";
                using var command = new MySqlCommand(sqlText, connection);
                command.Parameters.AddWithValue("@UseName", patient.Name?.Use);
                command.Parameters.AddWithValue("@Gender", patient.Gender);
                command.Parameters.AddWithValue("@Active", patient.Active);
                command.Parameters.AddWithValue("@FamilyName", patient.Name?.Family);
                if (patient.Name?.Given != null)
                    command.Parameters.AddWithValue("@GivenName", string.Join(' ', patient.Name.Given));
                else
                    command.Parameters.AddWithValue("@GivenName", "");
                command.Parameters.AddWithValue("@BirthDate", patient.BirthDate);
                await command.ExecuteNonQueryAsync();
            }
            return NoContent();
        }

        [HttpPut("UpdatePatient{id}")]
        public async Task<IActionResult> UpdatePatient(Guid id, Patient patient)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sqlText = "UPDATE Patient SET UseName = @UseName, FamilyName = @FamilyName, " +
                             "GivenName = @GivenName, Gender = @Gender, BirthDate = @BirthDate, Active = @Active " +
                             "WHERE Id = @Id";
                using var command = new MySqlCommand(sqlText, connection);
                command.Parameters.AddWithValue("@UseName", patient.Name?.Use);
                command.Parameters.AddWithValue("@FamilyName", patient.Name?.Family);
                if (patient.Name?.Given != null)
                    command.Parameters.AddWithValue("@GivenName", string.Join(' ', patient.Name.Given));
                else
                    command.Parameters.AddWithValue("@GivenName", "");
                command.Parameters.AddWithValue("@Gender", patient.Gender);
                command.Parameters.AddWithValue("@BirthDate", patient.BirthDate);
                command.Parameters.AddWithValue("@Active", patient.Active);
                command.Parameters.AddWithValue("@Id", id);
                await command.ExecuteNonQueryAsync();
            }
            return NoContent();
        }

        [HttpDelete("DeletePatient{id}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sqlText = "DELETE FROM Patient WHERE Id = @Id";
                using var command = new MySqlCommand(sqlText, connection);
                command.Parameters.AddWithValue("@Id", id);
                await command.ExecuteNonQueryAsync();
            }
            return NoContent();
        }

    }
}
