import { View, Text, StyleSheet } from "react-native";

import { COLORS } from "../constants/theme";

export default function Logo() {
    return (
        <View style={styles.container}>
            <Text style={styles.main}>ZMA</Text>
            <Text style={styles.subtitle}>Zene Minden Alkalomra</Text>
        </View>
    )
}

const styles = StyleSheet.create({
    container: {
        alignItems: 'center',
        justifyContent: 'center',
        height: 100,
        width: '50%',
        margin: 'auto',
    },
    main: {
        fontSize: 40,
        color: COLORS.purple,
        fontWeight: 'bold',
        textShadowColor: 'rgba(0, 0, 0, 0.2)',
        textShadowOffset: { width: 1, height: 1 },
        textShadowRadius: 2,
    },
    subtitle: {
        fontSize: 16,
        color: COLORS.deepPurple,
        textShadowColor: 'rgba(0, 0, 0, 0.2)',
        textShadowOffset: { width: 1, height: 1 },
        textShadowRadius: 2,
    }
});