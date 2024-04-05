using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prometoFoodTrucksBackend.Services.Context
{
    public class UserService
    {
        private readonly DataContext _context;


        public UserService(DataContext context)
        {
            _context = context;
        }


        public bool DoesUserExist(string Username)
        {
            //check if username exist
            // if 1 matches then return the item
            //if no item matches, then return null

            //SingleOrDefault is a method from Linq, a language integrated query similar to SQL.  It was introduced my microsoft as part of the .net framework. 
            //This code checks if there is a user in the user information managed by _context whose username matches the provided Username. If such a user is found, it returns true; otherwise, it returns false.
            return _context.UserInfo.SingleOrDefault(user => user.Username == Username) != null;
        }

        public bool AddUser(CreateAccountDTO UserToAdd)
        {
            //if user already exists
            //if does not exist create new account
            //else return false

            bool result = false;

            //if user doesn't exist, add user based on model
            if (!DoesUserExist(UserToAdd.Username))
            {
                UserModel newUser = new UserModel();


                var hashPassword = HashPassword(UserToAdd.Password);

                newUser.ID = UserToAdd.ID;
                newUser.Username = UserToAdd.Username;
                newUser.Salt = hashPassword.Salt;
                newUser.Hash = hashPassword.Hash;

                //adds newUser to the database
                _context.Add(newUser);

                //save into database, return of number of entries written into database
                result = _context.SaveChanges() != 0;
            }

            return result;
        }


        // HashedPassword = H( Salt + Password)

        public PasswordDTO HashPassword(string password)
        {


            PasswordDTO newHashPassword = new PasswordDTO();

            //create a byte array using 64 bytes of salt. 
            byte[] SaltByte = new byte[64];

            //this is our randomizer
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();

            //take our SaltByte and make sure it contains no zero's.  Making it more secure.
            provider.GetNonZeroBytes(SaltByte);


            //converting our salt into a string
            string salt = Convert.ToBase64String(SaltByte);

            //encode our password with salt into hash.  10000 times, where 10000 is a common starting point
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, SaltByte, 10000);

            //convert the resulting byte array as a string of 256 bytes
            string hash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

            //we can save our hash and salt as an instance of passwordDTO
            newHashPassword.Salt = salt;
            newHashPassword.Hash = hash;


            return newHashPassword;
        }



        // verify users password
        public bool VerifyUsersPassword(string? password, string? storedHash, string? storedSalt)
        {

            //encode salt back in the the original byte array

            byte[] SaltBytes = Convert.FromBase64String(storedSalt);

            //repeat process of taking passowrd entered and hashing it again
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, SaltBytes, 10000);

            //RRC289 object, retrieve the 256 bytes of hash, convert those into a string, assign the result to the newhash
            string newHash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

            return newHash == storedHash;
        }


        public IActionResult Login(LoginDTO User)
        {
            IActionResult Result = Unauthorized();

            //check if user exists
            if (DoesUserExist(User.Username))
            {
                //if true, continue with authentication
                // if true, store our user object

                UserModel founderUser = GetUserByUsername(User.Username);

                //check if password is correct
                if (VerifyUsersPassword(User.Password, founderUser.Hash, founderUser.Salt))
                {
                    // anyone with this code can access the login
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));

                    //sign in credentials
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    //generate new token and log user out after 30 mins
                    var tokeOptions = new JwtSecurityToken(
                        issuer: "http://localhost:5000",
                        audience: "http://localhost:5000",
                        claims: new List<Claim>(), // Claims can be added here if needed
                        expires: DateTime.Now.AddMinutes(30), // Set token expiration time (e.g., 30 minutes)
                        signingCredentials: signinCredentials // Set signing credentials
                    );

                    // Generate JWT token as a string
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                    // return JWT token through http response with status code 200
                    Result = Ok(new { Token = tokenString });
                }

                //Token:
                // asdasdlejwfoeiwj. = header
                // oisodcijosdijcodsj. Payload: contains claims such as expiration time
                // ;slakf;sdlofk;slfk;. = signature encrypts and comines header and payload using secret key

            }

            return Result;
        }


        public UserModel GetUserByUsername(string username)
        {
            return _context.UserInfo.SingleOrDefault(user => user.Username == username);
        }


        public bool UpdateUser(UserModel userToUpdate)
        {
            _context.Update<UserModel>(userToUpdate);
            return _context.SaveChanges() != 0;
        }


        public bool UpdateUsername(int id, string username)
        {
            //sending over just the id and username
            //we have to get the object to be updated

            UserModel foundUser = GetUserById(id);

            bool result = false;

            if (foundUser != null)
            {
                //a user was found
                // update founderuser object
                foundUser.Username = username;
                _context.Update<UserModel>(foundUser);
                result = _context.SaveChanges() != 0;
            }

            return result;
        }


        public UserModel GetUserById(int id)
        {
            return _context.UserInfo.SingleOrDefault(user => user.ID == id);
        }


        public bool DeleteUser(string userToDelete)
        {
           //we are only sending over the username
           //if username found found, delete user

           UserModel foundUser = GetUserByUsername(userToDelete);

           bool result = false;

            if(foundUser != null){
                //user was found

                _context.Remove<UserModel>(foundUser);
                result = _context.SaveChanges() != 0;
            }

           return result;
        }


        public UserIdDTO GetUserIdDTOByUsername(string username){

            UserIdDTO UserInfo = new UserIdDTO();

            //query through database to find the user
            UserModel foundUser = _context.UserInfo.SingleOrDefault(user => user.Username == username);

            UserInfo.UserId = foundUser.ID;
            UserInfo.PublisherName = foundUser.Username;

            return UserInfo;
        }
    }
}