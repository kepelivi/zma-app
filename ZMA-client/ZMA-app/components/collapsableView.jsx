import React, { useState } from 'react';
import { View, Text, TouchableWithoutFeedback, Animated, StyleSheet, FlatList } from 'react-native';

import { COLORS } from '../constants/theme';

export default function CollapsibleView({ title, songs }) {
    const [collapsed, setCollapsed] = useState(true);
    const [animation] = useState(new Animated.Value(0));

    const toggleCollapse = () => {
        if (collapsed) {
            Animated.timing(animation, {
                toValue: 1,
                duration: 300,
                useNativeDriver: true
            }).start();
        } else {
            Animated.timing(animation, {
                toValue: 0,
                duration: 300,
                useNativeDriver: true
            }).start();
        }
        setCollapsed(!collapsed);
    };

    const heightInterpolate = animation.interpolate({
        inputRange: [0, 1],
        outputRange: [0, 200]
    });

    return (
        <View style={styles.safeArea}>
            <TouchableWithoutFeedback onPress={toggleCollapse}>
                <View style={styles.header}>
                    <Text style={styles.main}>{title}</Text>
                </View>
            </TouchableWithoutFeedback>
            <Animated.View style={{ height: heightInterpolate }}>
                <FlatList
                    data={songs}
                    renderItem={({ item }) => <Text style={styles.title}>{item.title}</Text>}
                    keyExtractor={item => item.id}
                    contentContainerStyle={styles.titleContainer}
                />
            </Animated.View>
        </View>
    );
};

const styles = StyleSheet.create({
    header: {
        padding: 16,
        backgroundColor: COLORS.deepPurple,
        alignItems: 'center',
        cursor: 'pointer',
    },
    main: {
        fontSize: 24,
        fontWeight: 'bold',
        color: '#fff',
    },
    title: {
        fontSize: 18,
        fontWeight: 'bold',
        color: '#9c27b0',
        marginBottom: 8,
    },
    titleContainer: {
        alignItems: 'center',
    },
})