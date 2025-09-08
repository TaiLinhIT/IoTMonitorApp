// src/context/authStore.ts
let accessToken: string | null = null;

export const AuthStore = {
  getAccessToken: () => accessToken,
  setAuth: (data: { accessToken: string;}) => {
    accessToken = data.accessToken;
    
    // log ra khi setAuth được gọi
    // console.log("AuthStore updated:", {
    //   data
    // });
  },
  clearAuth: () => {
    accessToken = null;
  },
};
