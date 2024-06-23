import React from 'react';
import { render } from '@testing-library/react-native';
import Loading from '../../components/loading';

describe('Loading component', () => {
    test('should render given text argument', () => {
        const { getByText } = render(
            <Loading message="Hello" />,
        );

        const message = getByText("Hello");
        expect(message).toBeTruthy();
    });
    
    test('should render default parameter when no arguments given', () => {
        const { getByText } = render(
            <Loading />,
        );

        const message = getByText("Loading...");
        expect(message).toBeTruthy();
    });
});