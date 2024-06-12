import React from 'react';
import { render, fireEvent, waitFor } from '@testing-library/react-native';
import { notification } from 'antd';
import { useNavigation } from 'expo-router';
import Login from '../../components/login';

jest.mock('expo-router', () => ({
    useNavigation: jest.fn(),
}));

jest.mock('antd', () => ({
    notification: {
        success: jest.fn(),
        error: jest.fn(),
        config: jest.fn(),
    },
}));

describe('Login component', () => {
    const mockedNavigate = jest.fn();

    beforeEach(() => {
        useNavigation.mockReturnValue({ navigate: mockedNavigate });
        global.fetch = jest.fn();
        notification.success.mockClear();
        notification.error.mockClear();
    });

    afterEach(() => {
        jest.resetAllMocks();
    });

    test('should render login form correctly', () => {
        const { getByPlaceholderText, getByText } = render(<Login />);

        expect(getByPlaceholderText('EMAIL')).toBeTruthy();
        expect(getByPlaceholderText('JELSZÓ')).toBeTruthy();
        expect(getByText('Belépés')).toBeTruthy();
    });

    test('should show password when toggle is clicked', () => {
        const { getByPlaceholderText, getByText } = render(<Login />);

        const passwordInput = getByPlaceholderText('JELSZÓ');
        const toggleButton = getByText('O');

        fireEvent.press(toggleButton);
        expect(passwordInput.props.secureTextEntry).toBe(false);

        fireEvent.press(toggleButton);
        expect(passwordInput.props.secureTextEntry).toBe(true);
    });

    test('should show success notification and navigate on successful login', async () => {
        global.fetch.mockResolvedValue({
            ok: true,
            json: async () => ({ token: 'fake-token' }),
        });

        const { getByPlaceholderText, getByText } = render(<Login />);

        fireEvent.changeText(getByPlaceholderText('EMAIL'), 'test@gmail.com');
        fireEvent.changeText(getByPlaceholderText('JELSZÓ'), 'Password1234!');
        fireEvent.press(getByText('Belépés'));

        await waitFor(() => {
            expect(notification.success).toHaveBeenCalledWith({
                message: 'Sikeres bejelentkezés!',
            });
            expect(mockedNavigate).toHaveBeenCalledWith('party');
        });
    });

    test('should show error notification on failed login', async () => {
        global.fetch.mockResolvedValue({
            ok: false,
        });

        const { getByPlaceholderText, getByText } = render(<Login />);

        fireEvent.changeText(getByPlaceholderText('EMAIL'), 'test@gmail.com');
        fireEvent.changeText(getByPlaceholderText('JELSZÓ'), 'pwd');
        fireEvent.press(getByText('Belépés'));

        await waitFor(() => {
            expect(notification.error).toHaveBeenCalledWith({
                message: 'E-mail vagy jelszó hibás. Kérem próbálja újra!',
            });
            expect(mockedNavigate).not.toHaveBeenCalled();
        });
    });
});