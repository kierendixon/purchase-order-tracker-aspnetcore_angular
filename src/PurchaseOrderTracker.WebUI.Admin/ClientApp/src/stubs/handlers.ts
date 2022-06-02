import { rest } from 'msw';
import { userUrl } from '../config/api.config';

const oneDayInMs = 24 * 60 * 60 * 1000;

export const handlers = [
  rest.get(userUrl, (req, res, ctx) => {
    const currentDate = new Date();

    return res(
      ctx.status(200),
      ctx.json({
        pagedList: [
          { id: '1', userName: 'regular user' },
          { id: '2', userName: 'admin user', isAdmin: true },
          { id: '3', userName: 'locked user', lockoutEnd: new Date(currentDate.getTime() + oneDayInMs) },
        ],
      })
    );
  }),
];
