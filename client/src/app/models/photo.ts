export class Photo {
  id: number | null = null;
  url: string | null = null;

  constructor(init?: Partial<Photo>) {
    Object.assign(this, init);
  }
}

export const photosForTesting: Photo[] = [
  new Photo({ url: 'https://localhost:5001/api/Photos/1', id: 1, }),
  new Photo({ url: 'https://localhost:5001/api/Photos/2', id: 2, }),
  new Photo({ url: 'https://localhost:5001/api/Photos/3', id: 3, }),
  new Photo({ url: 'https://localhost:5001/api/Photos/4', id: 4, }),
];
