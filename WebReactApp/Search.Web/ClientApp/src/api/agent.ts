import axios, { AxiosError, AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { router } from "../routes/routes";
import { store } from "../stores/Store";
import { IndexData } from "../models/indexData";
 
axios.defaults.baseURL = '/api';

axios.interceptors.response.use(async response => {
     return response;
}, (error: AxiosError) => {
    const { data, status, config } = error.response as AxiosResponse;
    switch (status) {
        case 400:
            if (config.method === 'get' && data?.errors?.hasOwnProperty('id'))
            {
                router.navigate('/not-found');
            }
                        
            if (data?.errors) {
                const modalStateErrors = [];
                for(const key in data.errors) {
                    if (data.errors[key]) {
                        modalStateErrors.push(data.errors[key])
                    }
                }

                throw modalStateErrors.flat();
            } else {
                toast.error(data);
            }
            break;
        case 401:
            toast.error('unauthorized');
            break;
        case 403:
            toast.error('forbidden');
            break;
        case 404:
            router.navigate('/not-found'); 
            break;
        case 500:
            store.commonStore.setServerError(data);
            router.navigate('/server-error');
            break;
    }

    return Promise.reject(error);
})

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
    get: <T>(url: string) => axios.get<T>(url).then(responseBody),
    getFile: (url: string) => axios({ url: url, method: 'GET', responseType: 'blob'}).then(responseBody),   
    post: <T>(url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
    put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
    delete: <T>(url: string) => axios.delete<T>(url).then(responseBody),
}

const IndexApi = {
    getAll: () => requests.get<IndexData[]>('/IndexMaintenance/all'),
    // details: (id: string) => requests.get<Activity>(`/Activity/${id}`),
    // create: (activity: Activity) => requests.post<Activity>('/Activity', activity),
    // update: (activity: Activity) => requests.put<Activity>(`/Activity/${activity.id}`, activity),
    // delete: (id: number) => requests.delete<DeleteResponse>(`/Activity/${id}`),
}

const agent = {
    ActivityApi: IndexApi,
}

export default agent;