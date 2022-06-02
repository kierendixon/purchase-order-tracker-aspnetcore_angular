import { accountUrl } from '../../config/routing.config';

export default async function fetchWrapper(url: string, method: string, data: any): Promise<Response | undefined> {
  const resp = await fetch(url, {
    method: method,
    headers: {
      Accept: 'application/json',
      'Content-Type': 'application/json',
    },
    body: data ? JSON.stringify(data) : undefined,
  });

  if (resp.status === 401) {
    window.location.href = accountUrl;

    // todo
    //   return { success: false, errors: ['Session expired'] };
  } else {
    return resp;
  }
}
