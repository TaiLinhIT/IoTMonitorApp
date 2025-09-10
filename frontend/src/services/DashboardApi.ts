import privateApi from "./axiosPrivate";
const DashboardApi = {
    getDashboardData:  () => {
        const response =  privateApi.get('/Dashboard');
        return response;
    }
}
export default DashboardApi;