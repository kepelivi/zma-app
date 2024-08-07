import { View, StyleSheet, Image } from "react-native";

export default function Logo() {
    return (
        <View style={styles.container}>
            <Image source={require('../assets/logo.png')} style={styles.logo} />
        </View>
    )
}

const styles = StyleSheet.create({
    container: {
        alignItems: 'center',
        justifyContent: 'center',
        height: 100,
        margin: 'auto',
    },
    logo: {
        width: 200,
        height: 200,
    }
});