using ApiDevBP.Controllers;
using ApiDevBP.Entities;
using ApiDevBP.Models;
using SQLite;
using Serilog;
namespace ApiDevBP.Services
{
    public class UserService
    {
        private readonly SQLiteConnection _db;
        public UserService(SQLiteConnection db)
        {
            _db = db;
            _db.CreateTable<UserEntity>();
        }
        public bool SaveUser(UserModel user)
        {
            try
            {
                var result = _db.Insert(new UserEntity()
                {
                    Name = user.Name,
                    Lastname = user.Lastname
                }); ;

                if (result > 0)
                {
                    Log.Information("Usuario guardado: {@User}", user);
                }
                return result > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al guardar usuario.");
                return false;
            }
        }
        public bool UpdateUser(UserModel updatedUser)
        {
            try
            {
                // Recupera el usuario existente de la base de datos usando su ID
                var existingUser = _db.Table<UserEntity>().FirstOrDefault(u => u.Id == updatedUser.id);

                if (existingUser != null)
                {
                    // Actualiza las propiedades del usuario existente con los nuevos valores
                    existingUser.Name = updatedUser.Name;
                    existingUser.Lastname = updatedUser.Lastname;

                    // Realiza la actualización en la base de datos
                    var result = _db.Update(existingUser);

                    if (result > 0)
                    {
                        Log.Information("Usuario actualizado: {@User}", existingUser);
                    }

                    return result > 0; // Retorna true si la actualización fue exitosa
                }
                else
                {
                    Log.Warning("Usuario no encontrado para actualizar con ID: {UserId}", updatedUser.id);
                    return false; // Retorna false si el usuario no existe en la base de datos
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al intentar actualizar usuario.");
                return false; // Retorna false en caso de error
            }
        }


        public IEnumerable<UserEntity> GetUsers()
        {
            try
            {
                var users = _db.Query<UserEntity>("Select * from Users");
                return users;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al traer los usuarios");
                return Enumerable.Empty<UserEntity>();
            }
        }
        public IEnumerable<UserEntity> GetUserById(int Id)
        {
            try
            {
                var users = _db.Query<UserEntity>("SELECT * FROM Users WHERE Id = ?", Id);
                return users;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al traer los usuarios por ID.");
                return Enumerable.Empty<UserEntity>();
            }
        }

        public bool DeleteUsers()
        {
            try
            {
                // Borra todos los registros en la tabla Users
                _db.DeleteAll<UserEntity>();

                Log.Information("Base de datos vaciada correctamente.");

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al intentar vaciar la base de datos.");
                return false;
            }
        }
    }
}
