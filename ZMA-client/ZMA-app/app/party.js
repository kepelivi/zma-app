import { StyleSheet, View, Text, FlatList, Pressable, SafeAreaView } from 'react-native';
import { useState, useEffect } from 'react';

import { COLORS } from '../constants/theme';

import Logo from '../components/logo';

export default function PartyManager() {
    const [parties, setParties] = useState([]);
    const [loading, setLoading] = useState(true);

    async function fetchParties() {
        const res = await fetch(`http://localhost:5086/Party/GetParties`,
            {
                method: "GET",
                credentials: 'include',
                headers: { 'Content-type': 'application/json' }
            }
        );
        return await res.json();
    }

    useEffect(() => {
        fetchParties()
            .then(parties => {
                setParties(parties);
                setLoading(false);
            })
    }, []);

    if (loading) return <View><Text>Loading...</Text></View>

    return (
        <SafeAreaView>
            <Logo />
            <View style={styles.container}>
                <Text style={styles.main}>Bulik</Text>
            </View>
            <FlatList
                data={parties}
            />
        </SafeAreaView>
    )
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: COLORS.white,
        alignItems: 'center',
        justifyContent: 'center',
    },
    main: {
        fontSize: 25
    }
});