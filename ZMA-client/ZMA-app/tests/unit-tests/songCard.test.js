import React from 'react';
import { render, fireEvent, waitFor } from '@testing-library/react-native';
import SongCard from '../../components/songCard';
import '@testing-library/jest-native/extend-expect';

global.fetch = jest.fn();

describe('SongCard component', () => {
    beforeEach(() => {
        global.fetch.mockClear();
    });

    afterEach(() => {
        jest.resetAllMocks();
    });

    const song = {
        "id": 4,
        "title": "test",
        "requestTime": "2024-06-23T17:46:51.0098311",
        "accepted": false,
        "partyId": "42b467b5-baff-4e2d-7fd3-08dc8af01006"
      }

    test('should render test song correctly', () => {
        const { getByText } = render(<SongCard song={song} />)

        expect(getByText("test")).toBeTruthy();
    });
});