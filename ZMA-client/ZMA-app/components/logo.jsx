import { View, Text, StyleSheet } from "react-native";

import { COLORS } from "../constants/theme";

export default function Logo() {
    return (
        <View>
            <View style={styles.container}>
                <Text style={styles.main}>ZMA</Text>
            </View>
            <View>
            <Text>Zene Minden Alkalomra</Text>
            </View>
        </View>
    )
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: COLORS.purple,
        border: 'solid',
        borderColor: COLORS.purple,
        borderRadius: '4px',
        alignItems: 'center',
        justifyContent: 'center',
    },
    main: {
        fontSize: 30
    }
});