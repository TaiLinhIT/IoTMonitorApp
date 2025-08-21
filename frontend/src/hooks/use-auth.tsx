// import axios from "axios";
// // import { OAuth2Client } from "google-auth-library";
// import { useEffect, useState } from "react";
// import { useLocalStorage } from "./use-storage";

// export interface IUser {
//   email: string;
//   name: string;
//   picture: string;
// }

// // const client = new OAuth2Client(import.meta.env.VITE_GOOGLE_CLIENT_ID);

// export const useAuth = () => {
//   const [user, setUser] = useLocalStorage("user", null);

//   useEffect(() => {
//     const accessToken = localStorage.getItem("accessToken");
//     console.log(accessToken);
//     (async () => {
//       const response = await axios.get(
//         `https://www.googleapis.com/oauth2/v3/userinfo?access_token=${accessToken}`
//       );
//       //   localStorage.setItem("user", JSON.stringify(response.data));
//       //   console.log(response.data);
//       setUser(response.data);
//     })();
//   }, []);

//   //   console.log(user);

//   return user;
// };
