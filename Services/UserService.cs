using ApiDevBP.Entities;
using ApiDevBP.Models;
using Serilog;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace ApiDevBP.Services
{
    public class UserService
    {
        private readonly SQLiteConnection _db;

        public UserService(SQLiteConnection db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _db.CreateTable<UserEntity>();
        }

        public bool SaveUser(UserModel user)
        {
            try
            {
                var result = _db.Insert(new UserEntity
                {
                    Name = user.Name,
                    Lastname = user.Lastname
                });

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
                var existingUser = _db.Table<UserEntity>().FirstOrDefault(u => u.Id == updatedUser.id);

                if (existingUser != null)
                {
                    existingUser.Name = updatedUser.Name;
                    existingUser.Lastname = updatedUser.Lastname;

                    var result = _db.Update(existingUser);

                    if (result > 0)
                    {
                        Log.Information("Usuario actualizado: {@User}", existingUser);
                    }

                    return result > 0;
                }
                else
                {
                    Log.Warning("Usuario no encontrado para actualizar con ID: {UserId}", updatedUser.id);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al intentar actualizar usuario.");
                return false;
            }
        }

        public IEnumerable<UserEntity> GetUsers()
        {
            try
            {
                var users = _db.Query<UserEntity>("SELECT * FROM Users");
                return users;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al traer los usuarios.");
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
        public bool DeleteUserById(int id)
        {
            try
            {
                // Busca el usuario por ID
                var userToDelete = _db.Table<UserEntity>().FirstOrDefault(u => u.Id == id);

                if (userToDelete != null)
                {
                    // Borra el usuario de la base de datos
                    var result = _db.Delete(userToDelete);

                    if (result > 0)
                    {
                        Log.Information("Usuario eliminado por ID: {UserId}", id);
                    }

                    return result > 0; // Retorna true si la eliminación fue exitosa
                }
                else
                {
                    Log.Warning("Usuario no encontrado para eliminar con ID: {UserId}", id);
                    return false; // Retorna false si el usuario no existe en la base de datos
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al intentar eliminar usuario por ID.");
                return false; // Retorna false en caso de error
            }
        }
    }
}
