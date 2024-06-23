import React from 'react';
import { render, fireEvent, waitFor } from '@testing-library/react-native';
import SongCard from '../../components/songCard';
import '@testing-library/jest-native/extend-expect';

import { COLORS } from '../../constants/theme';

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

    test('should change color if song gets accepted', () => {
        const onAccept = jest.fn();
        const onDeny = jest.fn();

        const { getByText, rerender } = render(
            <SongCard song={song} onAccept={onAccept} onDeny={onDeny} />
        );

        const acceptButton = getByText("Elfogad");
        fireEvent.press(acceptButton);

        expect(onAccept).toHaveBeenCalled();

        const acceptedSong = { ...song, accepted: true };
        rerender(<SongCard song={acceptedSong} onAccept={onAccept} onDeny={onDeny} />);

        const card = getByText("test").parent.parent.parent.parent;

        expect(card).toHaveStyle({ backgroundColor: COLORS.greyish });
    });
});