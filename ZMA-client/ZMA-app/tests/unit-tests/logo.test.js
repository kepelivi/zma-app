import React from "react";
import { render } from "@testing-library/react-native";
import Logo from "../../components/logo";

test('Logo should render correctly', () => {
    const { getByText } = render(<Logo />);

    expect(getByText("ZMA")).toBeTruthy();
});