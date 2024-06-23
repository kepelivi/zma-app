import React from 'react';
import { render, fireEvent, waitFor } from '@testing-library/react-native';
import { useNavigation } from 'expo-router';
import PartyCard from '../../components/partyCard';
import { fetchAndSortParties, onDelete } from '../../app/party';

global.fetch = jest.fn();

jest.mock('../../app/party', () => ({
    fetchAndSortParties: jest.fn(),
    onDelete: jest.requireActual('../../app/party').onDelete,
}));

jest.mock('expo-router', () => ({
    useNavigation: jest.fn(),
    onDelete: jest.fn()
}));

describe('PartyCard component', () => {
    const mockedNavigate = jest.fn();

    beforeEach(() => {
        global.fetch.mockClear();
        fetchAndSortParties.mockClear();
        useNavigation.mockReturnValue({ navigate: mockedNavigate });
    });

    afterEach(() => {
        jest.resetAllMocks();
    });

    const party = {
        "id": "1715f388-ae16-4f5a-a905-08dc889f508b",
        "name": "test party",
        "host": null,
        "details": "party for test",
        "category": "test",
        "date": "2023-12-12T00:00:00",
        "queue": []
    }

    test('should render test party correctly', () => {
        const { getByText } = render(<PartyCard party={party} />)

        expect(getByText("test party")).toBeTruthy();
        expect(getByText("2023-12-12")).toBeTruthy();
    });

    test('should navigate to songRequests page of test party when pressed', () => {
        const { getByText } = render(<PartyCard party={party} onPress={() => mockedNavigate('songRequests', { id: party.id })} />);

        fireEvent.press(getByText("test party"));

        expect(mockedNavigate).toHaveBeenCalledWith('songRequests', { "id": `${party.id}` });
    });
});